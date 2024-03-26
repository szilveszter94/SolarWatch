import { Route, Routes } from 'react-router-dom'
import Home from './components/Home/Home';
import Authentication from './components/Authentication/Authentication';

import './App.scss'
import SunsetSunrise from './components/SunsetSunrise/SunsetSunrise';

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path='/authentication' element={<Authentication />} />
        <Route path='sunsetSunrise' element={<SunsetSunrise />} />
      </Routes>
    </>
  );
}

export default App
