import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { SnackbarContext } from "../../../assets/context/SnackbarContext";
import { fetchData } from "../../../utils/apiService";
import Loading from "../../Loading/Loading";
import SnackBar from "../../Snackbar/Snackbar";
import InputComponent from "../../InputComponent/InputComponent";

const sampleInfo = {
  loginPassword: "",
  loginEmail: "",
};

const Login = () => {
  const [userInfo, setUserInfo] = useState(sampleInfo);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { setSnackbar } = useContext(SnackbarContext);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: undefined,
  });

  const handleLogin = async (e) => {
    setLoading(true);
    e.preventDefault();
    try {
      const response = await fetchData({
        path: "/Auth/Login",
        method: "POST",
        body: {
          password: userInfo.loginPassword,
          email: userInfo.loginEmail,
        },
      });

      if (response.ok) {
        localStorage.setItem("accessToken", response.data.token);
        setSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });

        navigate("/");
        return;
      } else {
        setSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
        if (response.status === 403) {
          navigate("/activateAccount");
        }
      }
    } catch (error) {
      setLocalSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setUserInfo(sampleInfo);
    setLoading(false);
  };

  const handleChange = (e) => {
    const name = e.target.name;
    const value = e.target.value;
    setUserInfo({ ...userInfo, [name]: value });
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <div>
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />

      <h1>Login With Email</h1>
      <form onSubmit={handleLogin}>
        <InputComponent
          name="loginEmail"
          value={userInfo.loginEmail}
          type="email"
          placeholder="Email"
          onChange={handleChange}
        />
        <InputComponent
          name="loginPassword"
          value={userInfo.loginPassword}
          type="password"
          placeholder="Password"
          onChange={handleChange}
        />
        <div className="text-center">
          <button className="fs-2 mt-3 btn btn-outline-light" type="submit">
            Login
          </button>
        </div>
      </form>
    </div>
  );
};

export default Login;
