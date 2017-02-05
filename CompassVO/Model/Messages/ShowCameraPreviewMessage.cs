namespace CompassVO.Model.Messages
{
  public class ShowCameraPreviewMessage
  {
    public bool ShowCamera { get; set; }

    public ShowCameraPreviewMessage(bool showCamera)
    {
      ShowCamera = showCamera;
    }
  }
}