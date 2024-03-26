/* eslint-disable react/prop-types */
import "./InputComponent.scss";

const InputComponent = ({ name, value, placeholder, type, onChange }) => {
  return (
    <>
      <label htmlFor={name} className="form-label text-light fs-4">
        {placeholder}:
      </label>
      <input
        onChange={onChange}
        id={name}
        value={value}
        name={name}
        type={type}
        required
        placeholder={placeholder}
        className="form-control custom-input fs-4"
      />
    </>
  );
};

export default InputComponent;
