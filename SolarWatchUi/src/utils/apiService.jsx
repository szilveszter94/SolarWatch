const baseUrl = "http://localhost:5083";

export const fetchData = async ({ path, method, body }) => {
  try {
    const url = `${baseUrl}${path}`;
    const token = localStorage.getItem("accessToken");
    const options = {
      method: method,
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    };
    if (body !== null && body !== undefined) {
      options.body = JSON.stringify(body);
    }
    const response = await fetch(url, options);
    const data = await response.json();
    return { ok: true, data: data.data, message: data.message };
  } catch (error) {
    console.log(error);
    return { ok: false, message: "Error: The server is not responding." };
  }
};
