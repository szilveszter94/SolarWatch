import Navbar from "../../Navbar/Navbar";
import Footer from "../../Footer/Footer";
import InputComponent from "../../InputComponent/InputComponent";
import "./LocationCreator.scss";
import { useContext, useEffect, useState } from "react";
import { fetchData } from "../../../utils/apiService";
import SnackBar from "../../Snackbar/Snackbar";
import { useParams, useNavigate } from "react-router-dom";
import { UserContext } from "../../../assets/context/UserContext";
import Loading from "../../Loading/Loading";

const sampleLocation = { city: "", lat: "", lon: "" };

const LocationCreator = () => {
  const [locationInfo, setLocationInfo] = useState(sampleLocation);
  const { currentUser, loading } = useContext(UserContext);
  const [currentPageLoading, setCurrentPageLoading] = useState(false);
  const { id } = useParams();
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  useEffect(() => {
    const loadEditObject = async () => {
      try {
        setCurrentPageLoading(true);
        const response = await fetchData({
          path: `/SolarWatch/GetLocationDataById/${id}`,
          method: "GET",
          body: null,
        });
        if (response.ok) {
          setLocationInfo(response.data);
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
      setCurrentPageLoading(false);
    };
    if (id) {
      loadEditObject();
    } else {
      setLocationInfo(sampleLocation);
    }
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      id ? (locationInfo.id = id) : "";
      const path = id
        ? "/SolarWatch/UpdateLocationData"
        : "/SolarWatch/AddLocationData";
      const method = id ? "PATCH" : "POST";
      const response = await fetchData({
        path: path,
        method: method,
        body: locationInfo,
      });
      if (response.ok) {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
        navigate("/controlPanel?option=2");
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
    setLocationInfo(sampleLocation);
  };

  const handleChange = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setLocationInfo({ ...locationInfo, [key]: value });
  };

  if (loading || currentPageLoading) {
    return <Loading />;
  }

  if (!currentUser || (currentUser && currentUser.role !== "Admin")) {
    navigate("/");
  }

  return (
    <div className="location-creator-container vh-100">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <Navbar />
      <div className="location-creator-content">
        <div className="container mt-5">
          <div className="text-center my-5">
            <h1>Create new city information</h1>
          </div>
          <form onSubmit={handleSubmit}>
            <div className="row justify-content-center">
              <div className="col-md-5">
                <InputComponent
                  name="city"
                  value={locationInfo.city}
                  placeholder="City"
                  type="text"
                  onChange={handleChange}
                />
                <InputComponent
                  name="lat"
                  value={locationInfo.lat}
                  placeholder="Latitude"
                  type="number"
                  onChange={handleChange}
                />
                <InputComponent
                  name="lon"
                  value={locationInfo.lon}
                  placeholder="Longitude"
                  type="number"
                  onChange={handleChange}
                />
              </div>
              <div className="text-center">
                <button
                  className="btn btn-lg btn-outline-light mt-3"
                  type="submit"
                >
                  Submit
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default LocationCreator;
