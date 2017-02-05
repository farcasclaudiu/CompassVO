namespace CompassVO.Model.Messages
{
  public class UpdateCompassUIMessage
  {
    private UpdateCompassUIMessage()
    {
    }

    public UpdateCompassUIMessage(double angle)
    {
      Angle = angle;
    }

    public double Angle { get; set; }
  }
}