/* eslint-disable react-hooks/exhaustive-deps */
import Navbar from "../Navbar/Navbar";
import Footer from "../Footer/Footer";
import "./SunsetSunrise.scss";
import InputComponent from "../InputComponent/InputComponent";
import { useContext, useState } from "react";
import { fetchData } from "../../utils/apiService";
import SnackBar from "../Snackbar/Snackbar";
import { formatDate, formatTime } from "../../utils/helperFunctions";
import { useNavigate } from "react-router-dom";
import { UserContext } from "../../assets/context/UserContext";
import Loading from "../Loading/Loading";
import TextField from "@mui/material/TextField";
import Autocomplete from "@mui/material/Autocomplete";

const SunsetSunrise = () => {
  const [cityName, setCityName] = useState("");
  const [date, setDate] = useState("");
  const [cityInfo, setCityInfo] = useState(false);
  const [suggestions, setSuggestions] = useState([]);
  const navigate = useNavigate();
  const { loading, currentUser } = useContext(UserContext);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleDateChange = (e) => {
    const value = e.target.value;
    setDate(value);
  };

  const handleInputChange = async (city) => {
    setCityName(city);
    if (city.length > 2) {
      try {
        const response = await fetchData({
          path: `/SolarWatch/GetCitiesForAutocomplete?suggestion=${city}`,
          method: "GET",
          body: null,
        });
        const duplicatedCity = response.data.find(i => i.label == city);
        if (duplicatedCity){
          setSuggestions(response.data);
        } else {
          setSuggestions([...response.data, {id: "userInput", label: city}]);
        }
      } catch (error) {
        console.error("Error fetching suggestions:", error);
      }
    } else {
      setSuggestions([{id: "userInput", label: city}]);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const path = `/SolarWatch/GetSunsetSunrise?city=${cityName}&date=${date}`;
      const response = await fetchData({
        path: path,
        method: "GET",
        body: null,
      });
      if (response.ok) {
        setCityInfo(response.data);
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
      } else {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
      }
    } catch (error) {
      setLocalSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setCityName("");
    setDate("");
  };

  if (loading) {
    return <Loading />;
  }

  if (!loading && !currentUser) {
    navigate("/authentication");
  }

  return (
    <div className="sunset-container vh-100">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <Navbar />
      <div className="sunset-content">
        <div className="container mt-5">
          <div className="text-center">
            <div className="row">
              <div className="col-md-6">
                <form onSubmit={handleSubmit}>
                  <div className="col-md-10">
                    <label
                      htmlFor="city"
                      className="form-label text-light fs-4"
                    >
                      City:
                    </label>
                    <Autocomplete
                      className="bg-light text-dark"
                      disablePortal
                      id="combo-box-demo"
                      value={cityName}
                      options={suggestions}
                      isOptionEqualToValue={(option, value) => () =>
                        option.id == value.id}
                      onInputChange={(e, value) => handleInputChange(value)}
                      renderInput={(params) => (
                        <TextField
                          {...params}
                          required
                          value={cityName}
                          onChange={(e) => handleInputChange(e.target.value)}
                          label="City"
                        />
                      )}
                    />
                    <InputComponent
                      name="date"
                      value={date}
                      placeholder="Date"
                      type="date"
                      onChange={handleDateChange}
                    />
                    <button
                      type="submit"
                      className="btn btn-lg mt-4 btn-outline-light"
                    >
                      Get Sunset Sunrise Data
                    </button>
                  </div>
                </form>
              </div>
              <div className="col-md-6">
                {cityInfo && (
                  <div className="text-light border rounded mt-4 p-1">
                    <h1 className="mb-2">{cityInfo.city}</h1>
                    <h3>{formatDate(cityInfo.date)}</h3>
                    <h4>Sunrise: {formatTime(cityInfo.sunrise)}</h4>
                    <h4>Sunset: {formatTime(cityInfo.sunset)}</h4>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default SunsetSunrise;
