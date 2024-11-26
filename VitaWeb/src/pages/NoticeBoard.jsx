import { useEffect, useState } from "react";
import NoticeList from "../components/NoticeBoard/NoticeList";
import { IoIosAddCircleOutline } from "react-icons/io";
import moment from "moment";
import { makeServer } from "../API/mockAPI";

// makeServer();

moment.updateLocale("da-DK", {
	culture: "da-DK",
});

const initialNotices = [
	{
		id: 1,
		title: "Velkommen til opslagstavlen",
		content:
			"Her kan du skrive beskeder til de andre brugere. Husk at være sød og høflig",
		createdAt: moment().format("DD-MM-YY - HH:mm"),
	},
	{
		id: 2,
		title: "Fælles Rengøringsdag",
		content:
			"Kom og vær med til fælles rengøringsdag på lørdag kl. 9! Lad os sammen gøre kvarteret pænt og rent. Vi mødes ved parkens indgang – handsker og affaldsposer vil blive uddelt. Vi glæder os til at se jer!",
		createdAt: moment().add(-2, "day").format("DD-MM-YY - HH:mm"),
	},
	{
		id: 3,
		title: "Nye Yoga Hold Starter Snart",
		content:
			"Vi er glade for at kunne annoncere nye yoga-hold, der starter i næste uge! Holdene vil blive afholdt hver tirsdag og torsdag kl. 18. Alle niveauer er velkomne. Husk at medbringe en måtte og vand!",
		createdAt: moment().add(-1, "day").format("DD-MM-YY - HH:mm"),
	},
	{
		id: 4,
		title: "Fælles Morgenmad",
		content:
			"Kom og nyd en hyggelig morgenmad med dine naboer i morgen kl. 9.30! Vi vil servere pandekager, frisk frugt og kaffe. Alle er velkomne!",
		createdAt: moment().add(-3, "hours").format("DD-MM-YY - HH:mm"),
	},
];

const NoticeBoard = () => {
	const [notices, setNotices] = useState(null);

	const handleAddNotice = async () => {
		const response = await fetch("api/notices", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
		});
		const data = await response.json();
		console.log(data);
		setNotices([...notices, data.notice]);
	};

	useEffect(() => {
		const getNotices = async () => {
			const response = await fetch("api/notices");
			const data = await response.json();
			setNotices(data.notices);
			console.log(data);
		};
		getNotices();
	}, []);

	if (!notices) {
		return (
			<div className="w-full h-screen flex items-center justify-center">
				<h1>Loading...</h1>
			</div>
		);
	}

	return (
		<div>
			<div className="w-full flex flex-col justify-center items-center p-5 shadow bg-gray-200">
				<h1 className="text-6xl tracking-tighter">Opslagstavlen</h1>
			</div>
			<div className="px-20 py-5 flex flex-col items-center">
				<div className="w-full grid grid-cols-2 grid-flow-row gap-3 mb-5 justify-center">
					<NoticeList notices={notices} />
				</div>
				<IoIosAddCircleOutline
					onClick={handleAddNotice}
					className="text-4xl cursor-pointer active:scale-100 hover:rotate-90 hover:scale-125 transition-all"
				/>
			</div>
		</div>
	);
};

export default NoticeBoard;
