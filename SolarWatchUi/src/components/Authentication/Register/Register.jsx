import { useState } from "react";
import { fetchData } from "../../../utils/apiService";
import Loading from "../../Loading/Loading";
import SnackBar from "../../Snackbar/Snackbar";
import InputComponent from "../../InputComponent/InputComponent";

const sampleInfo = {
  registerUsername: "",
  registerPassword: "",
  confirmPassword: "",
  registerEmail: "",
};

const Register = () => {
  const [userInfo, setUserInfo] = useState(sampleInfo);
  const [loading, setLoading] = useState(false);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: undefined,
  });

  const handleRegister = async (e) => {
    setLoading(true);
    if (userInfo.confirmPassword !== userInfo.registerPassword) {
      setLocalSnackbar({
        open: true,
        message: "Passwords do not match.",
        type: "error",
      });
      return;
    }
    e.preventDefault();
    try {
      const response = await fetchData({
        path: "/Auth/Register",
        method: "POST",
        body: {
          username: userInfo.registerUsername,
          email: userInfo.registerEmail,
          password: userInfo.registerPassword,
        },
      });
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
      <h1>Register</h1>
      <form onSubmit={handleRegister}>
        <InputComponent
          name="registerEmail"
          value={userInfo.registerEmail}
          type="email"
          placeholder="Email"
          onChange={handleChange}
        />
        <InputComponent
          name="registerUsername"
          value={userInfo.registerUsername}
          type="text"
          placeholder="Username"
          onChange={handleChange}
        />
        <InputComponent
          name="registerPassword"
          value={userInfo.registerPassword}
          type="password"
          placeholder="Password"
          onChange={handleChange}
        />
        <InputComponent
          name="confirmPassword"
          value={userInfo.confirmPassword}
          type="password"
          placeholder="Confirm password"
          onChange={handleChange}
        />
        <div className="text-center">
          <button type="submit" className="fs-2 mt-5 btn btn-outline-light">
            Register
          </button>
        </div>
      </form>
    </div>
  );
};

export default Register;
