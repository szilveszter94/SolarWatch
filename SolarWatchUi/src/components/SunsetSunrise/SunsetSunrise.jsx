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

const sampleData = { city: "", date: "" };

const SunsetSunrise = () => {
  const [info, setInfo] = useState(sampleData);
  const [cityInfo, setCityInfo] = useState(false);
  const navigate = useNavigate();
  const { loading, currentUser } = useContext(UserContext);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleChange = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setInfo({ ...info, [key]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const path = `/SolarWatch/GetSunsetSunrise?city=${info.city}&date=${info.date}`;
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
    setInfo(sampleData);
  };

  if (loading){
    return <Loading />
  }

  if (!loading && !currentUser){
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
                    <InputComponent
                      name="city"
                      value={info.city}
                      placeholder="City"
                      type="text"
                      onChange={handleChange}
                    />
                    <InputComponent
                      name="date"
                      value={info.date}
                      placeholder="Date"
                      type="date"
                      onChange={handleChange}
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
