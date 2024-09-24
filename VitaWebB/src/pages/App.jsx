import { Route, Routes } from "react-router-dom";
import Home from './home'
import Videos from './Videos'
import NavBar from "../Components/NavBar";

function App() {

  return (
    <>
		<NavBar/>
		<Routes>
			<Route path="/" element={<Home />} />
			<Route path="/videos" element={<Videos/>} />
		</Routes>
    </>
  );
}

export default App
