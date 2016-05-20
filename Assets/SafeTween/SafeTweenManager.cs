using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace SafeTween
{
    public class SafeTweenManager : MonoBehaviour
    {
        static SafeTweenManager _instance;

        public static SafeTweenManager Instance
        {
            get
            {
                if (_instance == null)
                    CreateManager();
                return _instance;
            }
        }

        static Dictionary<object,List<Tweener>> tweeners;
        static List<Tweener> playing;
        static List<Tweener> adding;
        static List<Tweener> stopping;

        static void CreateManager()
        {
            if (_instance != null)
                return;

            var go = new GameObject("[SafeTweenmanager]");
            _instance = go.AddComponent<SafeTweenManager>();
            Init();
        }

        static void Init()
        {
            tweeners = new Dictionary<object, List<Tweener>>();
            adding = new List<Tweener>();
            playing = new List<Tweener>();
            stopping = new List<Tweener>();
        }

        public static void ForceStop(Tweener tweener)
        {
            CreateManager();
            adding.Remove(tweener);
            stopping.Add(tweener);
        }

        public static void Stop(Tweener tweener)
        {
            CreateManager();
            stopping.Add(tweener);
        }

        public static void Play(Tweener tweener)
        {
            CreateManager();
            adding.Add(tweener);
        }

        void Update()
        {
            foreach (var tweener in stopping)
                playing.Remove(tweener);
            stopping.Clear();

            foreach (var tweener in adding)
                if (!playing.Contains(tweener))
                    playing.Add(tweener);
            adding.Clear();

            foreach (var tweener in playing)
                tweener.Update();
        }
    }

    public class Tweener
    {
        List<TweenProperty> tweenProperties;
        public float speed = 1;
        float time;
        float duration;
        bool isReverse;

        protected Action OnForwardAction;
        protected Action OnBackwardAction;
        protected Action OnForwardCompletedAction;
        protected Action OnBackwardCompletedAction;

        public float normalizedTime { get { return time / duration; } }

        public Tweener()
        {
            tweenProperties = new List<TweenProperty>();
        }

        public void Add(TweenProperty tweenProperty)
        {
            tweenProperty.tweener = this;
            tweenProperty.isPlaying = true;
            tweenProperties.Add(tweenProperty);

            if (tweenProperty.endTime > duration)
                duration = tweenProperty.endTime;
        }

        /// <summary>
        /// Reset to start position
        /// </summary>
        public void Reset()
        {
            foreach (var property in tweenProperties)
                property.UpdateValue(0);
            time = 0;
        }

        /// <summary>
        /// Set to end position
        /// </summary>
        public void SetToEnd()
        {
            foreach (var property in tweenProperties)
                property.UpdateValue(1);
            time = duration;
        }

        /// <summary>
        /// Play from start
        /// </summary>
        public void Play()
        {
            foreach (var property in tweenProperties)
                property.UpdateValue(0);
            PlayForward(0);
        }

        /// <summary>
        /// Play forward from current position
        /// </summary>
        public void PlayForward()
        {
            PlayForward(time);
        }

        /// <summary>
        /// Play forward from position
        /// </summary>
        /// <param name="time">position</param>
        public void PlayForward(float time)
        {
            foreach (var property in tweenProperties)
            {
                property.isPlaying = true;
                property.isReverse = false;
            }

            if (OnForwardAction != null)
                OnForwardAction();

            this.time = time;
            isReverse = false;
            SafeTweenManager.Play(this);
        }

        /// <summary>
        /// Play backward from end
        /// </summary>
        public void Reverse()
        {
            foreach (var property in tweenProperties)
                property.UpdateValue(1);
            PlayBackward(duration);
        }

        /// <summary>
        /// Play backward from current position
        /// </summary>
        public void PlayBackward()
        {
            PlayBackward(time);
        }

        /// <summary>
        /// Play backward from position
        /// </summary>
        /// <param name="time">position</param>
        public void PlayBackward(float time)
        {
            foreach (var property in tweenProperties)
            {
                property.isPlaying = true;
                property.isReverse = true;
            }

            if (OnBackwardAction != null)
                OnBackwardAction();

            time = duration;
            isReverse = true;
            SafeTweenManager.Play(this);
        }

        /// <summary>
        /// Stop animation at current position
        /// </summary>
        public void Stop()
        {
            SafeTweenManager.Stop(this);
        }

        /// <summary>
        /// Stop animation at current position 
        /// and prevent previous call of Play() on the same frame from starting
        /// </summary>
        public void ForceStop()
        {
            SafeTweenManager.ForceStop(this);
        }

        public void SetDuration(float duration)
        {
            this.duration = Mathf.Clamp(duration, this.duration, float.MaxValue);
        }

        public void Update()
        {
            if (isReverse)
                time -= Time.deltaTime * speed;
            else
                time += Time.deltaTime * speed;

            foreach (var tweenProperty in tweenProperties)
                if (tweenProperty.isPlaying)
                    tweenProperty.Update(time);
           
            if (!isReverse && normalizedTime > 1)
                OnForwardComplete();
            if (isReverse && normalizedTime < 0)
                OnBackwardComplete();
        }

        void OnForwardComplete()
        {
            Stop();
            if (OnForwardCompletedAction != null)
                OnForwardCompletedAction();
        }

        void OnBackwardComplete()
        {
            Stop();
            if (OnBackwardCompletedAction != null)
                OnBackwardCompletedAction();
        }

        public virtual Tweener OnForward(Action OnForwardAction)
        {
            this.OnForwardAction = OnForwardAction;
            return this;
        }

        public virtual Tweener OnBackword(Action OnBackwardAction)
        {
            this.OnBackwardAction = OnBackwardAction;
            return this;
        }

        public virtual Tweener OnForwardComplete(Action OnBackwardCompletedAction)
        {
            this.OnForwardCompletedAction = OnBackwardCompletedAction;
            return this;
        }

        public virtual Tweener OnBackwardComplete(Action OnForwardCompletedAction)
        {
            this.OnBackwardCompletedAction = OnForwardCompletedAction;
            return this;
        }
    }

    public abstract class TweenProperty
    {
        public Tweener tweener;
        public bool isPlaying;
        public bool isReverse;

        public float startTime;
        public float endTime;

        protected Action OnPlayCompleted;
        protected Action OnReverseCompleted;

        public void Play()
        {
            isPlaying = true;
            tweener.Play();
        }

        public void PlayReverse()
        {
            isPlaying = true;
            tweener.Play();
        }

        public virtual void Update(float time)
        {
            if (time >= startTime && time <= endTime)
                UpdateValue((time - startTime) / (endTime - startTime));

            if (!isReverse && time > endTime)
                OnPlayComplete();
            if (isReverse && time < startTime)
                OnReverseComplete();
        }

        public virtual void UpdateValue(float normalizedTime)
        {
            throw new NotImplementedException();
        }

        void OnPlayComplete()
        {
            UpdateValue(1);

            if (OnPlayCompleted != null)
                OnPlayCompleted();
            isPlaying = false;
        }

        void OnReverseComplete()
        {
            UpdateValue(0);

            if (OnReverseCompleted != null)
                OnReverseCompleted();
            isPlaying = false;
        }

        public virtual TweenProperty OnReverseComplete(Action OnPlayReverseCompleteAction)
        {
            this.OnReverseCompleted = OnPlayReverseCompleteAction;
            return this;
        }

        public virtual TweenProperty OnPlayComplete(Action OnPlayCompleteAction)
        {
            this.OnPlayCompleted = OnPlayCompleteAction;
            return this;
        }
    }

    public class TweenFontSize :TweenProperty
    {
        Text text;
        public int start;
        public int end;

        public TweenFontSize(Text text, int start, int end, float startTime, float endTime)
        {
            this.text = text;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            text.fontSize = Mathf.RoundToInt(Mathf.Lerp(start, end, time));
        }
    }

    public class TweenImageFill : TweenProperty
    {
        Image image;
        public float start;
        public float end;

        public TweenImageFill(Image image, float start, float end, float startTime, float endTime)
        {
            this.image = image;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            image.fillAmount = Mathf.Lerp(start, end, time);
        }
    }

    public class TweenImageColor : TweenProperty
    {
        Image image;
        public Color start;
        public Color end;

        public TweenImageColor(Image image, Color start, Color end, float startTime, float endTime)
        {
            this.image = image;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            image.color = Color.Lerp(start, end, time);
        }
    }

    public class TweenImageAlpha : TweenProperty
    {
        Image image;
        public float start;
        public float end;

        public TweenImageAlpha(Image image, float start, float end, float startTime, float endTime)
        {
            this.image = image;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            var color = image.color;
            color.a = Mathf.Lerp(start, end, time);
            image.color = color;
        }
    }

    public class TweenRawImageColor : TweenProperty
    {
        RawImage rawImage;
        public Color start;
        public Color end;

        public TweenRawImageColor(RawImage rawImage, Color start, Color end, float startTime, float endTime)
        {
            this.rawImage = rawImage;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            rawImage.color = Color.Lerp(start, end, time);
        }
    }



    public class TweenLocalPosition : TweenProperty
    {
        Transform transform;
        public Vector3 start;
        public Vector3 end;

        public TweenLocalPosition(Transform transform, Vector3 start, Vector3 end, float startTime, float endTime)
        {
            this.transform = transform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            transform.localPosition = Vector3.Lerp(start, end, time);
        }
    }

    public class TweenPosition : TweenProperty
    {
        Transform transform;
        public Vector3 start;
        public Vector3 end;

        public TweenPosition(Transform transform, Vector3 start, Vector3 end, float startTime, float endTime)
        {
            this.transform = transform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            transform.position = Vector3.Lerp(start, end, time);
        }
    }

    public class TweenLocalRotation : TweenProperty
    {
        Transform transform;
        public Vector3 start;
        public Vector3 end;

        public TweenLocalRotation(Transform transform, Vector3 start, Vector3 end, float startTime, float endTime)
        {
            this.transform = transform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            transform.localRotation = Quaternion.Euler(Vector3.Slerp(start, end, time));
        }
    }

    public class TweenAnchorPosition : TweenProperty
    {
        RectTransform rectTransform;
        public Vector2 start;
        public Vector2 end;

        public TweenAnchorPosition(RectTransform rectTransform, Vector2 start, Vector2 end, float startTime, float endTime)
        {
            this.rectTransform = rectTransform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, time);
        }
    }

    public class TweenSizeDelta : TweenProperty
    {
        RectTransform rectTransform;
        public Vector2 start;
        public Vector2 end;

        public TweenSizeDelta(RectTransform rectTransform, Vector2 start, Vector2 end, float startTime, float endTime)
        {
            this.rectTransform = rectTransform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            rectTransform.sizeDelta = Vector2.Lerp(start, end, time);
        }
    }

    public class TweenSizeDeltaX : TweenProperty
    {
        RectTransform rectTransform;
        public float start;
        public float end;

        public TweenSizeDeltaX(RectTransform rectTransform, float start, float end, float startTime, float endTime)
        {
            this.rectTransform = rectTransform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            var sizeDelta = rectTransform.sizeDelta;
            rectTransform.sizeDelta = Vector2.Lerp(new Vector2(start, sizeDelta.y), new Vector2(end, sizeDelta.y), time);
        }
    }

    public static class ImageExtension
    {
        public static TweenImageFill Fill(this Image image, float target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenImageFill = new TweenImageFill(image, image.fillAmount, target, delay, duration + delay);
            tweener.Add(tweenImageFill);
            tweener.PlayForward();
            return tweenImageFill;
        }

        //        public static TweenImageFill Fill(this Image image, Color target, float duration, float delay = 0)
        //        {
        //            var tweener = new Tweener();
        //            var tweenProperty = new TweenImageColor(image, image.color, target, delay, duration + delay);
        //            tweener.Add(tweenProperty);
        //            tweener.PlayForward();
        //            return tweenProperty;
        //        }
    }

    public static class TransformExtension
    {
        public static TweenLocalPosition MoveLocalPositionRelative(this RectTransform rectTransform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalPosition(rectTransform, rectTransform.localPosition, rectTransform.localPosition + target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenLocalPosition MoveLocalPosition(this RectTransform rectTransform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalPosition(rectTransform, rectTransform.localPosition, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenLocalPosition MoveLocalPosition(this Transform transform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalPosition(transform, transform.localPosition, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenPosition MovePosition(this Transform transform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenPosition(transform, transform.position, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }
    }

    public static class RectTransformExtension
    {
        public static TweenAnchorPosition MoveAnchorPosition(this RectTransform rectTransform, Vector2 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenAnchorPosition(rectTransform, rectTransform.anchoredPosition, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }
    }
}