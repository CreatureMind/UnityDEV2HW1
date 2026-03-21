using System;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace UI.Vinyl
{
    public class VinylRack : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer clickMeSign;
        [SerializeField] private CinemachineVirtualCamera selectCam;
        [SerializeField] private int newPriority = 100;
        [SerializeField] private LayerMask vinylRackLayer;
        [SerializeField] private TMP_Text infoText;
    
        [Header("Tween Settings")]
        [SerializeField] private float moveHeight = 0.2f;
        [SerializeField] private float duration = 0.5f;
    
        [FormerlySerializedAs("songs")]
        [Header("Vinyls")]
        [SerializeField] private SongWraperSO songWraperSo;
        [SerializeField] private Vinyl[] vinyls;
    
        public static event Action OnSelected;
    
        private Camera _camera;
        private bool _isClicked;
        private int _scrollOffset;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _camera = Camera.main;
            selectCam.Priority = 0;

            infoText.gameObject.SetActive(false);
            _scrollOffset = 0;
            InitializeVinyls();
        
            DoHover();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isClicked)
            {
                HandleScroll();
                infoText.gameObject.SetActive(true);
            }
            else
            {
                infoText.gameObject.SetActive(false);
            }
        }

        private void HandleScroll()
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
        
            if (scroll == 0f) return;
        
            int totalSongs = songWraperSo.songs.Length;
            int maxOffset = Mathf.Max(0, totalSongs - vinyls.Length);

            if (scroll > 0f)
            {
                _scrollOffset = Mathf.Max(0, _scrollOffset - 1);
            }
            else if (scroll < 0f)
            {
                _scrollOffset = Mathf.Min(maxOffset, _scrollOffset + 1);
            }

            RefreshVinyls();
        }

        private void DoHover()
        {
            if (!_isClicked)
            {
                clickMeSign.gameObject.SetActive(true);
                clickMeSign.transform.DOLookAt(_camera.transform.position, 0, AxisConstraint.Y);
                clickMeSign.transform.DOMove(clickMeSign.transform.position + Vector3.up * moveHeight, duration)
                    .SetEase(Ease.OutCubic)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void InitializeVinyls()
        {
            RefreshVinyls();
        }
    
        private void RefreshVinyls()
        {
            for (int i = 0; i < vinyls.Length; i++)
            {
                int songIndex = i + _scrollOffset;

                if (songIndex < songWraperSo.songs.Length)
                {
                    vinyls[i].gameObject.SetActive(true);
                    vinyls[i].SetSong(songWraperSo.songs[songIndex]);
                }
                else
                {
                    vinyls[i].gameObject.SetActive(false);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var clickedObject = 1 << gameObject.layer;
            
            if ((clickedObject & vinylRackLayer.value) != 0 && !_isClicked)
            {
                _isClicked = true;
                clickMeSign.gameObject.SetActive(false);
                selectCam.Priority = newPriority;
                OnSelected?.Invoke();
            }
        }
        
        private void OnEscape()
        {
            selectCam.Priority = 0;
            _isClicked = false;
            OnSelected?.Invoke();
        }
    }
}
