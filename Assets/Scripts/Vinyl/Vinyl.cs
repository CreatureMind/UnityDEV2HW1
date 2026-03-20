using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vinyl
{
    public class Vinyl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Sprite noPreview;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private TMP_Text songName;
    
        [Header("Tween Settings")]
        [SerializeField] private float moveHeight = 0.1f;
        [SerializeField] private float duration = 0.3f;

        private Camera _camera;
        private Vector3 _startPos;
        private Quaternion _startRot;
        private bool _isSelected = false;
    
        public static event Action<Song> OnSelectedSong;

        private Song Song { get; set; }
    
        private bool _isHovered = false;

        private void OnEnable()
        {
            VinylRack.OnSelected += ToggleSelected;
        }
    
        private void OnDisable()
        {
            VinylRack.OnSelected -= ToggleSelected;
        }

        private void Awake()
        {
            _camera = Camera.main;
            _startPos = sr.transform.localPosition;
            _startRot = sr.transform.localRotation;
        
            songName.gameObject.SetActive(false);
        }
    
        public void SetSong(Song song)
        {
            Song = song;
            if (Song)
            {
                sr.sprite = song.preview ? song.preview : noPreview;
                songName.text = song.songName;
            }
        }

        private void ToggleSelected()
        {
            _isSelected = !_isSelected;
            gameObject.layer = _isSelected ? 0 : 2;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isSelected) return;
        
            sr.transform.DOLocalMove(_startPos + Vector3.up * moveHeight + Vector3.forward * moveHeight, duration);
            sr.transform.DOLookAt(_camera.transform.position, duration);
            songName.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isSelected) return;
        
            sr.transform.DOLocalMove(_startPos, duration);
            sr.transform.DOLocalRotateQuaternion(_startRot, duration);
            songName.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isSelected) return;
        
            OnSelectedSong?.Invoke(Song);
        }
    }
}
