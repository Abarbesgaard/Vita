import { Outlet } from "react-router-dom";

const Placeholder = ({ text }) => {
	return (
		<div className="w-screen h-screen flex flex-col items-center justify-center">
			{text}
			<Outlet />
		</div>
	);
};

export default Placeholder;
