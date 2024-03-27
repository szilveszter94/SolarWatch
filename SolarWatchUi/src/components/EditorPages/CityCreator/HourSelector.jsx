/* eslint-disable react/prop-types */
const hours = [];
const minutes = [];
for (let i = 0; i < 24; i++) {
  hours.push(i);
}
for (let i = 0; i < 60; i++) {
  minutes.push(i);
}

const HourSelector = ({
  hourName,
  minuteName,
  hourValue,
  minuteValue,
  handleChange,
}) => {
  return (
    <div className="d-flex justify-content-around">
      <div className="me-2">
        <label htmlFor={hourName}>Hour:</label>
        <select
          id={hourName}
          className="form-select"
          name={hourName}
          required
          value={hourValue}
          onChange={handleChange}
        >
          <option disabled value="">
            Hour
          </option>
          {hours.map((hour) => (
            <option key={hour} value={hour}>
              {hour}
            </option>
          ))}
        </select>
      </div>
      <div className="ms-2">
        <label htmlFor={minuteName}>Minute:</label>
        <select
          id={minuteName}
          name={minuteName}
          className="form-select"
          value={minuteValue}
          onChange={handleChange}
        >
          <option disabled value="">
            Minute
          </option>
          {minutes.map((minute) => (
            <option key={minute} value={minute}>
              {minute}
            </option>
          ))}
        </select>
      </div>
    </div>
  );
};

export default HourSelector;
