import { fetchData } from "./apiService";

export const logoutUser = async () => {
  try {
    localStorage.removeItem("accessToken");
    return true;
  } catch (error) {
    console.log("Failed to logout.");
    return false;
  }
};

export const getAuth = async () => {
  try {
    const token = localStorage.getItem("accessToken");
    const userAuth = await fetchData({
      path: "/Auth/Validate",
      method: "POST",
      body: { token },
    });
    return userAuth;

  } catch (error) {
    console.error("Error. Server not responding.");
    return false;
  }
};
