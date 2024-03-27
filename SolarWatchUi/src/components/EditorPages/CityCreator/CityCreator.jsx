import Navbar from "../../Navbar/Navbar";
import Footer from "../../Footer/Footer";
import "./CityCreator.scss";
import InputComponent from "../../InputComponent/InputComponent";
import { useState } from "react";
import HourSelector from "./HourSelector";
import { useContext } from "react";
import { UserContext } from "../../../assets/context/UserContext";
import Loading from "../../Loading/Loading";
import { useNavigate } from "react-router-dom";
import { convertTo12HourFormat } from "../../../utils/helperFunctions";
import { fetchData } from "../../../utils/apiService";
import SnackBar from "../../Snackbar/Snackbar";

const hours = [];
const minutes = [];
for (let i = 0; i < 24; i++) {
  hours.push(i);
}
for (let i = 0; i < 60; i++) {
  minutes.push(i);
}

const sampleCityInfo = {
  date: "",
  city: "",
  sunriseHour: "",
  sunriseMinute: "",
  sunsetHour: "",
  sunsetMinute: "",
};

const CityCreator = () => {
  const [cityInfo, setCityInfo] = useState(sampleCityInfo);
  const { currentUser, loading } = useContext(UserContext);
  const navigate = useNavigate("/");
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleChange = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setCityInfo({ ...cityInfo, [key]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const convertedSunrise = convertTo12HourFormat(
        `${cityInfo.sunriseHour}`,
        cityInfo.sunriseMinute
      );
      const convertedSunset = convertTo12HourFormat(
        `${cityInfo.sunsetHour}`,
        cityInfo.sunsetMinute
      );
      const body = {
        city: cityInfo.city,
        date: cityInfo.date,
        sunrise: convertedSunrise,
        sunset: convertedSunset,
      };
      const response = await fetchData({
        path: "/SolarWatch/AddCityInformation",
        method: "POST",
        body,
      });
      console.log(response);
      if (response.ok) {
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
        message: "Server not responding",
        type: "error",
      });
    }
    setCityInfo(sampleCityInfo);
  };

  if (loading) {
    return <Loading />;
  }

  if (!currentUser || (currentUser && currentUser.role !== "Admin")) {
    navigate("/");
  }

  return (
    <div className="city-creator-container vh-100">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <Navbar />
      <div className="city-creator-content">
        <form onSubmit={handleSubmit}>
          <div className="container mt-5">
            <div className="text-center my-5">
              <h1>Create new city information</h1>
            </div>
            <div className="row justify-content-center">
              <div className="col-md-5">
                <InputComponent
                  name="city"
                  value={cityInfo.city}
                  placeholder="City"
                  type="text"
                  onChange={handleChange}
                />
                <InputComponent
                  name="date"
                  value={cityInfo.date}
                  placeholder="Date"
                  type="date"
                  onChange={handleChange}
                />
              </div>
            </div>
            <div className="row justify-content-center my-3">
              <div className="mt-2 col-md-5">
                <div className="border rounded p-2">
                  <div className="text-center">
                    <label className="form-label text-light fs-4">
                      Sunrise
                    </label>
                  </div>
                  <div className="d-flex justify-content-around">
                    <HourSelector
                      handleChange={handleChange}
                      hourName="sunriseHour"
                      minuteName="sunriseMinute"
                      hourValue={cityInfo.sunriseHour}
                      minuteValue={cityInfo.sunriseMinute}
                    />
                  </div>
                  <hr />
                  <div className="text-center">
                    <label className="form-label text-light fs-4 mb-2">
                      Sunset
                    </label>
                  </div>
                  <div className="d-flex justify-content-around mb-2">
                    <HourSelector
                      handleChange={handleChange}
                      hourName="sunsetHour"
                      minuteName="sunsetMinute"
                      hourValue={cityInfo.sunsetHour}
                      minuteValue={cityInfo.sunsetMinute}
                    />
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="text-center mt-3 mb-5">
            <button className="btn btn-lg btn-outline-light">Submit</button>
          </div>
        </form>
      </div>
      <Footer />
    </div>
  );
};

export default CityCreator;
