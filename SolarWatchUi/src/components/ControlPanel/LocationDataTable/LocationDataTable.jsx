import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { fetchData } from "../../../utils/apiService";

/* eslint-disable react/prop-types */
const LocationDataTable = ({
  fetchedData,
  setFetchedData,
  setLocalSnackbar,
  navigate
}) => {
  const handleDelete = async (id) => {
    try {
      const response = await fetchData({
        path: `/SolarWatch/DeleteLocationData?id=${id}`,
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
    navigate(`/edit/location/${id}`)
  };

  return (
    <table className="table table-dark table-striped">
      <thead>
        <tr>
          <th scope="col">Id</th>
          <th scope="col">City</th>
          <th scope="col">Latitude</th>
          <th scope="col">Longitude</th>
          <th scope="col">Edit</th>
          <th scope="col">Delete</th>
        </tr>
      </thead>
      <tbody>
        {fetchedData.map((location) => (
          <tr key={location.id}>
            <th scope="row">{location.id}</th>
            <td>{location.city}</td>
            <td>{location.lat}</td>
            <td>{location.lon}</td>
            <td>
              <button
                onClick={() => handleEdit(location.id)}
                className="btn btn-sm btn-success"
              >
                <FontAwesomeIcon icon={faEdit} />
              </button>
            </td>
            <td>
              <button
                onClick={() => handleDelete(location.id)}
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

export default LocationDataTable;
