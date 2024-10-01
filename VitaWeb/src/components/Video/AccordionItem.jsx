import { AnimatePresence, motion } from "framer-motion";
import { useState } from "react";
import { FaChevronDown } from "react-icons/fa";

const AccordionItem = ({ title, description, children }) => {
	const [isOpen, setIsOpen] = useState(false);
	const [isVisible, setIsVisible] = useState(false);

	const toggleAccordion = () => {
		setIsOpen(!isOpen);
	};

	const truncatedDesc =
		description.length > 50 ? description.slice(0, 40) + "..." : description;

	return (
		<>
			<motion.div
				className="border border-black rounded-lg text-gray-200 p-2 h-20 cursor-pointer flex-shrink-0 flex flex-col justify-center mb-2 bg-gray-900"
				onClick={toggleAccordion}
			>
				<div className="flex">
					<div className="w-1/2">
						<p className="font-bold">{title}</p>
						<p>{truncatedDesc}</p>
					</div>
					<div
						className="mx-auto w-20 flex flex-col items-center"
						onClick={(e) => {
							e.stopPropagation();
							setIsVisible(!isVisible);
						}}
					>
						<p className="text-center">
							{isVisible ? "Synlig" : "Ikke synlig"}
						</p>
						<motion.div
							className={`bg-gray-600 w-10 h-5 rounded-full px-1 flex items-center shadow-inner ${
								isVisible ? "justify-end" : "justify-start"
							}`}
							initial={false}
							animate={{
								backgroundColor: isVisible ? "#9ca3af" : "#374151",
							}}
						>
							<motion.button
								className="bg-white h-4 w-4 rounded-full shadow-md"
								layout
								initial={false}
								animate={{
									backgroundColor: isVisible ? "white" : "#4b5563",
								}}
								transition={{ type: "spring", stiffness: 700, damping: 30 }}
							></motion.button>
						</motion.div>
					</div>
					<motion.div
						className="ml-auto mr-4 my-auto flex items-center"
						initial={false}
						animate={{
							rotate: isOpen ? 180 : 0,
						}}
						transition={{ duration: 0.3 }}
					>
						<FaChevronDown className=" h-6 w-6" />
					</motion.div>
				</div>
			</motion.div>
			<AnimatePresence>{isOpen && children}</AnimatePresence>
		</>
	);
};

export default AccordionItem;
