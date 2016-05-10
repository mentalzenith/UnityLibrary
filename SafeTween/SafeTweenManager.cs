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

        static Dictionary<object,Tweener> tweeners;
        static List<Tweener> playing;
        static List<Tweener> adding;
        static List<Tweener> stopping;

        static void CreateManager()
        {
            var go = new GameObject("[SafeTweenmanager]");
            _instance = go.AddComponent<SafeTweenManager>();
            Init();
        }

        static void Init()
        {
            tweeners = new Dictionary<object, Tweener>();
            adding = new List<Tweener>();
            playing = new List<Tweener>();
            stopping = new List<Tweener>();
        }

        public void Add(object o, Tweener tweener)
        {
            //tweeners.Add(o, tweener);
            Play(tweener);
        }

        public static void Stop(Tweener tweener)
        {
//            playing.Remove(tweener);
            stopping.Add(tweener);
        }

        public static void Play(Tweener tweener)
        {
            adding.Add(tweener);
        }

        void Update()
        {
            playing.AddRange(adding);
            adding.Clear();

            foreach (var tweener in playing)
                tweener.Update();

            foreach (var tweener in stopping)
                playing.Remove(tweener);
            stopping.Clear();
        }
    }

    public class Tweener
    {
        List<TweenPropertyBase> tweenProperties;
        float time;

        public Tweener()
        {
            tweenProperties = new List<TweenPropertyBase>();
        }

        public void Add(TweenPropertyBase tweenProperty)
        {
            tweenProperty.tweener = this;
            tweenProperty.isPlaying = true;
            tweenProperties.Add(tweenProperty);
        }

        public void Play()
        {
            time = 0;
            SafeTweenManager.Play(this);
        }

        public void Stop()
        {
            SafeTweenManager.Stop(this);
        }

        public void Update()
        {
            time += Time.deltaTime;
            foreach (var tweenProperty in tweenProperties)
                if (tweenProperty.isPlaying)
                    tweenProperty.Update(time);
        }
    }

    public abstract class TweenPropertyBase
    {
        public Tweener tweener;
        public float delay;
        public float duration;
        protected Action OnCompleteAction;

        public bool isPlaying;

        public abstract void Update(float time);

        public virtual void OnComplete()
        {
            if (OnCompleteAction != null)
                OnCompleteAction();
            isPlaying = false;
        }

        public virtual TweenPropertyBase SetDelay(float delay)
        {
            this.delay = delay;
            return this;
        }

        public virtual TweenPropertyBase SetCompleteAction(Action OnCompleteAction)
        {
            this.OnCompleteAction = OnCompleteAction;
            return this;
        }
    }

    public class TweenImageFill:TweenPropertyBase
    {
        Image image;
        public float start;
        public float end;

        public TweenImageFill(Image image, float end, float duration)
        {
            this.image = image;
            this.start = image.fillAmount;
            this.end = end;
            this.duration = duration;
        }

        public override void Update(float time)
        {
            if (time <= delay)
                return;
            
            image.fillAmount = Mathf.Lerp(start, end, time - delay / duration);

            if (time >= duration + delay)
                OnComplete();
        }
    }

    public class TweenPositionRelative:TweenPropertyBase
    {
        RectTransform rectTransform;
        public Vector3 start;
        public Vector3 endRelative;

        public TweenPositionRelative(RectTransform rectTransform, Vector3 end, float duration)
        {
            this.rectTransform = rectTransform;
            this.start = rectTransform.localPosition;
            this.endRelative = end;
            this.duration = duration;
            this.OnCompleteAction = OnCompleteAction;
        }

        public override void Update(float time)
        {
            if (time <= delay)
                return;

            rectTransform.localPosition = Vector3.Lerp(start, start + endRelative, time - delay / duration);

            if (time >= duration + delay)
                OnComplete();
        }
    }

    public static class ImageExtension
    {
        public static TweenImageFill Fill(this Image image, float target, float duration)
        {
            var tweener = new Tweener();
            var tweenImageFill = new TweenImageFill(image, target, duration);
            tweener.Add(tweenImageFill);
            SafeTweenManager.Instance.Add(image, tweener);
            return tweenImageFill;
        }
    }

    public static class TransformExtension
    {
        public static TweenPositionRelative MovePositionRelative(this RectTransform rectTransform, Vector3 target, float duration)
        {
            var tweener = new Tweener();
            var tweenPositionRelative = new TweenPositionRelative(rectTransform, target, duration);
            tweener.Add(tweenPositionRelative);
            SafeTweenManager.Instance.Add(rectTransform, tweener);
            return tweenPositionRelative;
        }
    }
}