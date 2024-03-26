import { useContext } from "react";
import { SnackbarContext } from "../../assets/context/SnackbarContext";
import SnackBar from "../Snackbar/Snackbar";
import Navbar from "../Navbar/Navbar";
import Footer from "../Footer/Footer";
import "./Home.scss";

const Home = () => {
  const { snackbar, setSnackbar } = useContext(SnackbarContext);

  return (
    <div className="home-container vh-100">
      <SnackBar
        {...snackbar}
        setOpen={() => setSnackbar({ ...snackbar, open: false })}
      />
      <Navbar />
      <div className="home-content">
        <div className="container mt-5">
          <div className="text-center">
            <h1 className="title mb-4">
              Witness the Beauty of Sunrise and Sunset with{" "}
              <span className="brand-name">SolarWatch</span>!
            </h1>
            <h2 className="lead-text mb-4">
              Explore the mesmerizing moments when the world wakes up and bids
              farewell to the day.
            </h2>
            <h2 className="lead-text">
              With SolarWatch, immerse yourself in the splendor of nature`s
              daily spectacle. Discover the precise timings of sunrise and
              sunset for your favorite cities around the globe.
            </h2>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default Home;
