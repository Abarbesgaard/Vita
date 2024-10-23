import { Route, Routes } from "react-router-dom";
import Home from './home'
import Videos from './Videos'
import NavBar from "../Components/NavBar";
import Calender from './Calender/Calender.jsx'
import {Inject, ScheduleComponent, Day, Week, Month} from '@syncfusion/ej2-react-schedule';

function App() {

  return (
    <>
		<NavBar/>
		<Routes>
			<Route path="/" element={<Home />} />
			<Route path="/videos" element={<Videos/>} />
			<Route path="/calender" element={<Calender/>} />
			<Route path="*" element={<h1>Not Found</h1>} />
		</Routes>
    </>
  );
}

export default App
