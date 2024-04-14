export const formatDate = (dateString) => {
  const date = new Date(dateString);

  const options = {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric",
  };
  const formattedDate = date.toLocaleDateString("en-US", options);
  return formattedDate;
};

export const convertTimeToDateTime = (timeString, dateObj) => {
  const date = dateObj.split("T")[0];
  return new Date(`${date} ` + timeString);
};

export const formatTime = (timeString) => {
  const date = new Date("1/1/2013 " + timeString);
  const formattedTime = date.toLocaleTimeString("en-US", {
    hour: "2-digit",
    minute: "2-digit",
    hour12: false,
  });
  return formattedTime;
};

export const convertTo12HourFormat = (hour24, minutes) => {
  const meridiem = hour24 >= 12 ? "PM" : "AM";
  let hour12 = hour24 % 12;
  hour12 = hour12 === 0 ? 12 : hour12;
  const time12 = `${hour12}:${minutes
    .toString()
    .padStart(2, "0")}:00 ${meridiem}`;

  return time12;
};

export const extractFormObjectFromCityInformationData = (data) => {
  const sunriseTime = formatTime(data.sunrise).split(":");
  const sunsetTime = formatTime(data.sunset).split(":");
  const sampleCityInfo = {
    date: data.date.split("T")[0],
    city: data.city,
    sunriseHour: Number(sunriseTime[0]),
    sunriseMinute: Number(sunriseTime[1]),
    sunsetHour: Number(sunsetTime[0]),
    sunsetMinute: Number(sunsetTime[1]),
  };
  return sampleCityInfo;
};

export const renderForecastOptions = () => {
  const options = [];
  for (let i = 0; i < 31; i++) {
    options.push(
      <option key={i} value={i + 1}>
        {i + 1}
      </option>
    );
  }
  return options;
};

export const getSearchParam = (searchParams) => {
  const value = Number(searchParams.get("option"));
  if (value) {
    return value;
  }
  return 1;
};
