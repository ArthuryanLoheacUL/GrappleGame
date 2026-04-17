using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathPlayerAnalyser : MonoBehaviour
{
    public struct Frame
    {
        public Vector2 pos;
        public bool isGrabbed;
        public Vector2 posGrabbed;
        public bool dead;
    }

    public static PathPlayerAnalyser instance;

    [Header("Captures Params")]
    [SerializeField] private float delayImages = 0.5f;
    private Transform player;
    private bool recording = false;
    private float timerImages = 0;
    private List<Frame> current_positions = new List<Frame>();
    private List<List<Frame>> positions = new List<List<Frame>>();

    [Header("Show Params")]
    [SerializeField] private GameObject prefabLinePath;
    [SerializeField] private Color linePathColor;

    [SerializeField] private GameObject prefabLinePathCurrent;
    [SerializeField] private Color linePathCurrentColor;

    [SerializeField] private GameObject prefabLinePathBest;
    [SerializeField] private Color linePathBestColor;
    
    [SerializeField] private GameObject prefabCross;
    [SerializeField] private float delayShowPath = 0.05f;
    [SerializeField] private int showedImages = 5;
    private FinalCameraPos finalCameraPos;
    int idBest = -1;

    [Header("Ghost")]
    [SerializeField] private GameObject ghostPrefab;
    private GameObject playerGhost;
    private bool isActive = true;
    private Vector2 targetPos;
    private bool isGrabbed = false;
    private Vector2 prevPos;
    private float timerLerp = 0;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        finalCameraPos = GameObject.FindGameObjectWithTag("CineCam").GetComponent<FinalCameraPos>();
        LoadBestSave();
    }

    public void StartRecording()
    {
        recording = true;
        timerImages = 0;
    }
    public void StartNewRecording()
    {
        ClearImages();
        if (idBest == -1)
            LoadBestSave();
        StartRecording();
        CreateGhost();
    }

    public void StopRecording(bool _isBest = false, bool _death = false)
    {
        if (!recording)
            return;
        TakeImage(_death);
        recording = false;
        positions.Add(new List<Frame>(current_positions));
        if (_isBest)
        {
            idBest = positions.Count - 1;
            StartCoroutine(PlayerSaveBest(positions[idBest]));
        }
    }

    public void ClearImages()
    {
        current_positions = new List<Frame>();
    }

    public void ClearPaths()
    {
        positions.Clear();
        idBest = -1;
        ClearImages();
    }

    private void Update()
    {
        if (recording && player)
        {
            timerImages += Time.deltaTime;
            if (timerImages > delayImages)
            {
                TakeImage();
            }
        } else
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
        if (playerGhost)
        {
            UpdateGhost();
        }
    }

    void UpdateGhost()
    {
        playerGhost.SetActive(isActive);
        timerLerp += Time.deltaTime;
        if (positions.Count >= idBest && idBest != -1 &&
            positions[idBest].Count > 0 &&
            current_positions.Count > 0 &&
            current_positions.Count < positions[idBest].Count)
        {

            Frame _currentFrame = positions[idBest][current_positions.Count];
            if (targetPos != _currentFrame.pos)
            {
                timerLerp = 0;
                prevPos = playerGhost.transform.position;
                isGrabbed = _currentFrame.isGrabbed;
                if (isGrabbed)
                {
                    LineRenderer _l = playerGhost.GetComponent<LineRenderer>();
                    _l.enabled = true;
                    _l.positionCount = 2;
                    _l.SetPosition(1, _currentFrame.posGrabbed);
                } else
                {
                    playerGhost.GetComponent<LineRenderer>().enabled = false;
                }
            }
            targetPos = _currentFrame.pos;
        } else
        {
            playerGhost.SetActive(false);
        }
        playerGhost.transform.position = Vector2.Lerp(prevPos, targetPos, timerLerp / delayImages);
        if (isGrabbed)
        {
            LineRenderer _l = playerGhost.GetComponent<LineRenderer>();
            _l.SetPosition(0, playerGhost.transform.position);
        }
    }

    void TakeImage(bool _death = false)
    {
        if (!player)
            return;

        timerImages = 0;
        Frame _frame = new Frame();
        _frame.pos = player.position;
        _frame.isGrabbed = false;
        _frame.dead = _death;
        GunScript[] _guns = player.GetComponentsInChildren<GunScript>();
        foreach (GunScript _gun in _guns)
        {
            if (_gun.IsGrabbed())
            {
                _frame.isGrabbed = true;
                _frame.posGrabbed = _gun.grappledPoint;
            }
        }
        current_positions.Add(_frame);
    }

    public void ShowPath()
    {
        if (recording)
            StopRecording();
        finalCameraPos = GameObject.FindGameObjectWithTag("CineCam").GetComponent<FinalCameraPos>();

        for (int _i = 0; _i < positions.Count; _i++)
        {
            GameObject _prefab = prefabLinePath;
            Color _colorCross = linePathColor;
            if (_i == positions.Count - 1)
            {
                _prefab = prefabLinePathCurrent;
                _colorCross = linePathCurrentColor;
            }
            else if (_i == idBest)
            {
                _prefab = prefabLinePathBest;
                _colorCross = linePathBestColor;
            }

            StartCoroutine(ProgressiveShow(positions[_i], _prefab, _colorCross));
        }
    }

    IEnumerator ProgressiveShow(List<Frame> _positions, GameObject _prefab, Color _colorCross)
    {
        GameObject _linePath = Instantiate(_prefab, transform);
        LineRenderer _lineRenderer = _linePath.GetComponent<LineRenderer>();

        int _i = 0;
        foreach (Frame _segment in _positions)
        {
            _lineRenderer.positionCount = _i + 1;
            _lineRenderer.SetPosition(_i, _segment.pos);
            _lineRenderer.widthMultiplier = finalCameraPos.finalOrthographicSize / 40;
            if (_segment.dead)
            {
                GameObject _cross = Instantiate(prefabCross, _linePath.transform);
                _cross.transform.position = _segment.pos;
                _cross.transform.localScale = new Vector3(_lineRenderer.widthMultiplier, _lineRenderer.widthMultiplier, _lineRenderer.widthMultiplier) / 1.5f;
                _cross.GetComponent<SpriteRenderer>().color = _colorCross;
            }

            if (_i % showedImages == 0)
                yield return new WaitForSeconds(delayShowPath);
            _i++;
        }
    }

    public void HidePath()
    {
        StopAllCoroutines();
        int _childs = transform.childCount;
        for (int _i = 0; _i < _childs; _i++)
            Destroy(transform.GetChild(_i).gameObject);
    }

    IEnumerator PlayerSaveBest(List<Frame> _pos)
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_PB_Count", _pos.Count);
        for (int _i = 0; _i < _pos.Count; _i++)
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_x", _pos[_i].pos.x);
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_y", _pos[_i].pos.y);
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_grabbed", _pos[_i].isGrabbed ? 1 : 0);
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_PG_x", _pos[_i].posGrabbed.x);
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_PG_y", _pos[_i].posGrabbed.y);
            yield return null;
        }
    }

    void CreateGhost()
    {
        if (playerGhost == null)
            playerGhost = Instantiate(ghostPrefab);
        else
            playerGhost.SetActive(true);
    }

    void LoadBestSave()
    {
        List<Frame> _pos = new List<Frame>();
        int _c = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_PB_Count", -1);
        CreateGhost();
        for (int  _i = 0; _i < _c; _i++)
        {
            Frame _frame = new Frame();
            _frame.pos.x = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_x", 0);
            _frame.pos.y = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_y", 0);
            _frame.isGrabbed = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_grabbed", 0) == 1;
            _frame.posGrabbed.x = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_PG_x", 0);
            _frame.posGrabbed.y = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_PG_y", 0);
            _pos.Add(_frame);
        }
        positions.Add(_pos);
        idBest = positions.Count - 1;
    }
}
