namespace SCE
{
    public class RenderIntercept<T> : UIInterceptBase<T>
        where T : IRenderable
    {
        public RenderIntercept(T renderable)
            : base(renderable)
        {
        }

        public Action? OnRender;

        public IUpdateLimit? UpdateLimit;

        public override DisplayMapView GetMapView()
        {
            if (OnRender != null && (UpdateLimit?.OnUpdate() ?? true))
            {
                OnRender.Invoke();
            }
            return Renderable.GetMapView();
        }
    }
}
