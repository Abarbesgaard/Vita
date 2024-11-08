import { Outlet } from "react-router-dom";

const Placeholder = ({ text }) => {
	return (
		<div className="h-full flex items-center justify-center border-4">
			{text}
		</div>
	);
};

export default Placeholder;
