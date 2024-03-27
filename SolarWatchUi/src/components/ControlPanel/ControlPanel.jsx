import Navbar from "../Navbar/Navbar";
import Footer from "../Footer/Footer";

import "./ControlPanel.scss";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { fetchData } from "../../utils/apiService";
import Loading from "../Loading/Loading";
import CityInfoTable from "./CityInfoTable/CityInfoTable";
import { useContext } from "react";
import { UserContext } from "../../assets/context/UserContext";
import LocationDataTable from "./LocationDataTable/LocationDataTable";
import SnackBar from "../Snackbar/Snackbar";

const ControlPanel = () => {
  const [fetchedData, setFetchedData] = useState([]);
  const [option, setOption] = useState(1);
  const [loadingCurrentPage, setLoadingCurrentPage] = useState(false);
  const { loading, currentUser } = useContext(UserContext);
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  useEffect(() => {
    const fetchInformation = async () => {
      const path =
        option == 1
          ? "/SolarWatch/GetAllCityInformation"
          : "/SolarWatch/GetAllLocationData";
      setLoadingCurrentPage(true);
      try {
        const response = await fetchData({
          path,
          method: "GET",
          body: null,
        });
        if (response.ok) {
          setFetchedData(response.data);
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
      setLoadingCurrentPage(false);
    };
    fetchInformation();
  }, [option]);

  const handleNavigate = (path) => {
    navigate(path);
  };

  const handleChange = (e) => {
    setOption(e.target.value);
    setLoadingCurrentPage(true);
  };

  if (loadingCurrentPage || loading) {
    return <Loading />;
  }

  if (!currentUser || (currentUser && currentUser.role !== "Admin")) {
    navigate("/");
  }

  return (
    <div className="control-panel-container vh-100">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <Navbar />
      <div className="control-panel-content">
        <div className="container mt-5">
          <div className="mb-3">
            <button
              onClick={() => handleNavigate("/create/cityInformation")}
              className="btn btn-lg btn-outline-success mx-2"
            >
              Create new city
            </button>
            <button
              onClick={() => handleNavigate("/create/location")}
              className="btn btn-lg btn-outline-success mx-2"
            >
              Create new location
            </button>
          </div>
          <div className="text-center">
            <div className="col-md-4">
              <select
                value={option}
                onChange={handleChange}
                className="form-select form-select-lg my-4 bg-dark text-light"
              >
                <option value={1}>Show City Information</option>
                <option value={2}>Show Location Information</option>
              </select>
            </div>
          </div>
          {option == 1 ? (
            <CityInfoTable
              fetchedData={fetchedData}
              setLocalSnackbar={setLocalSnackbar}
              setFetchedData={setFetchedData}
            />
          ) : (
            <LocationDataTable
              fetchedData={fetchedData}
              setLocalSnackbar={setLocalSnackbar}
              setFetchedData={setFetchedData}
            />
          )}
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default ControlPanel;
