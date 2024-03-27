/* eslint-disable react/prop-types */
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { formatDate, formatTime } from "../../../utils/helperFunctions";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import { fetchData } from "../../../utils/apiService";
import { useNavigate } from "react-router-dom";

const CityInfoTable = ({ fetchedData, setLocalSnackbar, setFetchedData }) => {
  const navigate = useNavigate();

  const handleDelete = async (id) => {
    try {
      const response = await fetchData({
        path: `/SolarWatch/DeleteCityInformation?id=${id}`,
        method: "DELETE",
        body: null,
      });
      if (response.ok) {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
        const updatedData = [...fetchedData].filter((item) => item.id != id);
        setFetchedData(updatedData);
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
  };

  const handleEdit = (id) => {
    navigate(`/edit/cityInformation/${id}`);
  };

  return (
    <table className="table table-dark table-striped">
      <thead>
        <tr>
          <th scope="col">Id</th>
          <th scope="col">City</th>
          <th scope="col">Date</th>
          <th scope="col">Sunrise</th>
          <th scope="col">Sunset</th>
          <th scope="col">Edit</th>
          <th scope="col">Delete</th>
        </tr>
      </thead>
      <tbody>
        {fetchedData.map((city) => (
          <tr key={city.id}>
            <th scope="row">{city.id}</th>
            <td>{city.city}</td>
            <td>{formatDate(city.date)}</td>
            <td>{formatTime(city.sunrise)}</td>
            <td>{formatTime(city.sunset)}</td>
            <td>
              <button
                onClick={() => handleEdit(city.id)}
                className="btn btn-sm btn-success"
              >
                <FontAwesomeIcon icon={faEdit} />
              </button>
            </td>
            <td>
              <button
                onClick={() => handleDelete(city.id)}
                className="btn btn-sm btn-danger"
              >
                <FontAwesomeIcon icon={faTrash} />
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default CityInfoTable;
