import { Route, Routes } from 'react-router-dom'
import Home from './components/Home/Home';
import Authentication from './components/Authentication/Authentication';

import './App.scss'
import SunsetSunrise from './components/SunsetSunrise/SunsetSunrise';
import LocationCreator from './components/EditorPages/LocationCreator/LocationCreator';
import CityCreator from './components/EditorPages/CityCreator/CityCreator';
import ControlPanel from './components/ControlPanel/ControlPanel';

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path='/authentication' element={<Authentication />} />
        <Route path='/sunsetSunrise' element={<SunsetSunrise />} />
        <Route path='/create/location' element={<LocationCreator />} />
        <Route path='/create/cityInformation' element={<CityCreator />} />
        <Route path='/controlPanel' element={<ControlPanel />} />
      </Routes>
    </>
  );
}

export default App
