import image from "../../assets/loading.gif";

const Loading = () => {
  return (
    <div className="container mt-5">
      <div className="row d-flex justify-content-center align-items-center">
        <div className="col-md-6 text-center">
          <img className="img-fluid w-50" src={image} alt="Loading..." />
        </div>
      </div>
    </div>
  );
};

export default Loading;
