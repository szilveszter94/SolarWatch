import { useEffect, useContext } from "react";
import { SnackbarContext } from "../../assets/context/SnackbarContext";
import SnackBar from "../Snackbar/Snackbar";
import Navbar from "../Navbar/Navbar";
import Register from "./Register/Register";
import Login from "./Login/Login";
import Footer from "../Footer/Footer";
import "./Authentication.scss";

const LoginRegister = () => {
  const { snackbar, setSnackbar } = useContext(SnackbarContext);

  useEffect(() => {
    setTimeout(() => {
      setSnackbar({
        open: false,
        message: "",
        type: undefined,
      });
    }, 6000);
  }, [setSnackbar]);

  return (
    <div className="login-register-container">
      <SnackBar
        {...snackbar}
        setOpen={() => setSnackbar({ ...snackbar, open: false })}
      />
      <Navbar />
      <div className="login-register-content">
        <div className="container">
          <div className="row d-flex justify-content-between">
            <div className="col-md-5 my-5 mx-3">
              <Register />
            </div>
            <div className="col-md-5 my-5 mx-3">
              <Login />
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default LoginRegister;
