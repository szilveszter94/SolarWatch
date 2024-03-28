<a name="readme-top"></a>

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
-->

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>

  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

### Project Name: SolarWatch

### Description:
- SolarWatch is an ASP.NET Web API project that provides the sunrise and sunset times for a given city on a specific date, in local or UTC times. It integrates with the Weather API and the Sunset and Sunrise Times API to fetch data, and utilizes a SQL Server database to store information retrieved from the APIs.

### Key Features:

- City Search: Users can search for cities and view the corresponding sunrise and sunset times.
- API Integration: Utilizes Weather API and Sunset and Sunrise Times API for retrieving solar data.
- Database Storage: Stores fetched data in a SQL Server database to optimize performance and reduce API calls.
- Role-Based Access Control: Admins have CRUD (Create, Read, Update, Delete) permissions for managing sunrise and sunset data.
  
### Tech Stack:

- Frontend: React
  - Style: Bootstrap, SCSS
- Backend: ASP.NET Web API
- Database: SQL Server
- API Integration: Weather API, Sunset and Sunrise Times API 

### Installation

_Below is an example of how you can instruct your audience on installing and setting up your app. This template doesn't rely on any external dependencies or services._

1. Get API keys for the Weather API and the Sunset and Sunrise Times API.
2. Clone the repository:
   ```sh
   git@github.com:szilveszter94/SolarWatch.git
   ```
3. Install NPM packages
   ```sh
   cd SolarWatchUi
   npm install
   ```
4. Enter your secrets in `.env`
   ```js
     DB_CONNECTION_STRING="Your db connection string for sql server"
     API_KEY="You api key from openweathermap API"
   ```
5. Start the server and the ui
   ```sh
   dotnet run
   npm run dev
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

### Sign Up/Login:

- If you're a new user, sign up for an account using your email.
- If you already have an account, simply log in to access the full features of SolarWatch.
- 
### Search for Solar Data:

- Use the search functionality to find sunrise and sunset times for a specific city on a particular date.
  
### Admin Actions:

- Admins can manage sunrise and sunset data by performing CRUD operations through the provided API endpoints.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

![Screenshot](https://github.com/szilveszter94/SolarWatch/blob/main/SolarWatch.jpg)

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Sándor Szilveszter - [@Email](s.szilveszter1994@gmail.com)

Project Link: [https://github.com/szilveszter94/CoffeeAndWifi](https://github.com/szilveszter94/CoffeeAndWifi)

<p align="right">(<a href="#readme-top">back to top</a>)</p>
