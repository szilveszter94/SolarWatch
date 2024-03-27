import { Link, useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useContext } from "react";
import { logoutUser } from "../../utils/authService";
import { UserContext } from "../../assets/context/UserContext";
import { SnackbarContext } from "../../assets/context/SnackbarContext";
import { faDashboard, faHouse, faSun } from "@fortawesome/free-solid-svg-icons";
import "./Navbar.scss";

const Navbar = () => {
  const navigate = useNavigate();
  const { currentUser, setCurrentUser } = useContext(UserContext);
  const { setSnackbar } = useContext(SnackbarContext);
  const handleLogout = async () => {
    const isLoggedOut = await logoutUser();
    if (isLoggedOut) {
      setSnackbar({
        open: true,
        message: "Successfully logged out.",
        type: "info",
      });
      setCurrentUser(null);
      navigate("/");
      return;
    } else {
      setSnackbar({
        open: true,
        message: "Failed to log out.",
        type: "info",
      });
      navigate("/");
      return;
    }
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-container">
      <div className="container-fluid">
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarTogglerDemo03"
          aria-controls="navbarTogglerDemo03"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <Link className="navbar-brand ms-3 fs-3" to="/">
          <img className="logo-img" src="" />
        </Link>
        <div className="collapse navbar-collapse" id="navbarTogglerDemo03">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <Link className="nav-link ms-4 fs-3" to="/">
                Home <FontAwesomeIcon icon={faHouse} />
              </Link>
            </li>
            {currentUser && (
              <li className="nav-item">
                <Link className="nav-link ms-4 fs-3" to="/sunsetSunrise">
                  Sunset&Sunrise <FontAwesomeIcon icon={faSun} />
                </Link>
              </li>
            )}
            {currentUser && currentUser.role === "Admin" && (
              <>
                <li className="nav-item">
                  <Link className="nav-link ms-4 fs-3" to="/controlPanel">
                    Control panel <FontAwesomeIcon icon={faDashboard} />
                  </Link>
                </li>
              </>
            )}
          </ul>
          <div className="me-2 mb-2">
            {currentUser ? (
              <span className="nav-item">
                <button className="nav-link ms-4 fs-3" onClick={handleLogout}>
                  Logout
                </button>
              </span>
            ) : (
              <span className="nav-item">
                <Link className="nav-link ms-4 fs-3" to="/authentication">
                  Register/Login
                </Link>
              </span>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
