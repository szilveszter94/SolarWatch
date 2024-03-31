namespace SolarWatch.Model;

public class LocationData {
    public int Id { get; set; }
    public string City { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (LocationData)obj;
        return Id == other.Id &&
               City == other.City &&
               Math.Abs(Lat - other.Lat) < 1 &&
               Math.Abs(Lon - other.Lon) < 1;
    }
};