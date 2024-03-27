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

export const formatTime = (timeString) => {
  const date = new Date("1/1/2013 " + timeString);
  const formattedTime = date.toLocaleTimeString("en-US", {
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
    hour12: false,
  });
  return formattedTime;
};

export const convertTo12HourFormat = (hour24, minutes) => {
  const meridiem = hour24 >= 12 ? 'PM' : 'AM';
  let hour12 = hour24 % 12;
  hour12 = hour12 === 0 ? 12 : hour12;
  const time12 = `${hour12}:${minutes.toString().padStart(2, '0')}:00 ${meridiem}`;

  return time12;
}
