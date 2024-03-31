namespace SolarWatch.Model;

public class AutocompleteCityModel
{
    public int Id { get; set; }
    public string City { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (AutocompleteCityModel)obj;
        return Id == other.Id &&
               City == other.City;
    }
};