/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable react/prop-types */
import { Line } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  TimeSeriesScale,
} from "chart.js";
import moment from "moment";
import { useEffect, useState } from "react";
import "chartjs-adapter-moment";
import {
  convertTimeToDateTime,
  fixInCorrectDate,
} from "../../../utils/helperFunctions";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  TimeSeriesScale
);

const sunriseDataRaw = {
  labels: [],
  datasets: [
    {
      label: "Sunrise",
      data: [],
      fill: false,
      borderColor: "rgb(255, 99, 132)",
      tension: 0.1,
    },
  ],
};

const sunsetDataRaw = {
  labels: [],
  datasets: [
    {
      label: "Sunset",
      data: [],
      fill: false,
      borderColor: "rgb(54, 162, 235)",
      tension: 0.1,
    },
  ],
};


const SunsetSunriseChart = ({ props }) => {
  const [sunsetData, setSunsetData] = useState(sunsetDataRaw);
  const [sunriseData, setSunriseData] = useState(sunriseDataRaw);
  const [minSunset, setMinSunset] = useState(new Date());
  const [minSunrise, setMinSunrise] = useState(new Date());

  useEffect(() => {
    const labels = [];
    const sunriseDataExtracted = [];
    const sunsetDataExtracted = [];
    props.forEach((item) => {
      labels.push(moment(item.date).format("MMM DD"));
      sunriseDataExtracted.push(convertTimeToDateTime(item.sunrise, props[0].date));
      sunsetDataExtracted.push(convertTimeToDateTime(item.sunset, props[0].date));
    });
    const minimumSunriseDate = new Date(
      Math.min.apply(null, sunriseDataExtracted)
    );
    const minimumSunsetDate = new Date(
      Math.min.apply(null, sunsetDataExtracted)
    );

    setMinSunrise(minimumSunriseDate);
    setMinSunset(minimumSunsetDate);
    setSunsetData({
      ...sunsetData,
      labels: labels,
      datasets: [
        {
          ...sunsetData.datasets[0],
          data: sunsetDataExtracted,
        },
      ],
    });
    setSunriseData({
      ...sunriseData,
      labels: labels,
      datasets: [
        {
          ...sunriseData.datasets[0],
          data: sunriseDataExtracted,
        },
      ],
    });
  }, []);

  const getOptions = (isSunset) => {
    const minimumValue = isSunset
      ? moment(minSunset).subtract(15, "seconds").format("YYYY-MM-DD HH:mm:ss")
      : moment(minSunrise)
          .subtract(15, "seconds")
          .format("YYYY-MM-DD HH:mm:ss");

    return {
      responsive: true,
      plugins: {
        legend: {
          position: "top",
          labels: {
            color: "white",
            font: {
              size: 28,
            },
          },
        },
        tooltip: {
          callbacks: {
            label: function (tooltipItem) {
              return fixInCorrectDate(tooltipItem);
            },
          },
        },
      },
      scales: {
        y: {
          type: "timeseries",
          time: {
            unit: "hour",
            unitStepSize: 1000,
            displayFormats: {
              hour: "HH:mm",
            },
          },
          ticks: {
            color: "white",
            font: {
              size: 18,
            },
          },
          grid: {
            color: "rgba(255, 255, 255, 0.2)",
          },
          min: minimumValue,
        },
        x: {
          ticks: {
            color: "rgba(255, 255, 255, 0.7)",
            font: {
              size: 18,
            },
          },
          grid: {
            color: "rgba(255, 255, 255, 0.2)",
          },
        },
      },
    };
  };

  return (
    <div className="container text-light">
      <div>
        <Line className="d-flex" options={getOptions(true)} data={sunsetData} />
      </div>
      <hr />
      <div className="mb-5">
        <Line
          className="d-flex"
          options={getOptions(false)}
          data={sunriseData}
        />
      </div>
    </div>
  );
};

export default SunsetSunriseChart;
