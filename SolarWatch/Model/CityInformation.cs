namespace SolarWatch.Model;

public class CityInformation {
    public int Id { get; set; }
    public string City { get; set; }
    public DateTime Date { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    
    public override string ToString()
    {
        return $"Id: {Id}, City: {City}, Date: {Date}, Sunrise: {Sunrise}, Sunset: {Sunset}";
    }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (CityInformation)obj;
        return Id == other.Id &&
               City == other.City &&
               Date == other.Date &&
               Sunrise == other.Sunrise &&
               Sunset == other.Sunset;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, City, Date, Sunrise, Sunset);
    }
};