namespace CompassVO.Utils.Filters
{
  public class SimpleKalman
  {
    private double Q = 0.0003;//0.000001;
    private double R = 0.01;//0.01;
    private double P = 1, X = 0, K = 0;

    private void measurementUpdate()
    {
      K = (P + Q) / (P + Q + R);
      P = R * (P + Q) / (R + P + Q);
    }

    public void init(double initX)
    {
      X = initX;
    }

    public double update(double measurement)
    {
      measurementUpdate();
      double result = X + (measurement - X) * K;
      X = result;
      return result;
    }

    public double current()
    {
      return X;
    }
  }
}