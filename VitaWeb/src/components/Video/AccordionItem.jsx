import { AnimatePresence, motion } from "framer-motion";
import { useState } from "react";
import { FaChevronCircleDown } from "react-icons/fa";

const AccordionItem = ({ title, description, children }) => {
	const [isOpen, setIsOpen] = useState(false);

	const toggleAccordion = () => {
		setIsOpen(!isOpen);
	};

	const truncatedDesc =
		description.length > 50 ? description.slice(0, 50) + "..." : description;

	return (
		<>
			<motion.div
				className="border border-black rounded-lg text-gray-200 p-2 h-20 cursor-pointer flex-shrink-0 flex flex-col justify-center mb-2 bg-gray-900"
				onClick={toggleAccordion}
			>
				<div className="flex">
					<div>
						<p className="font-bold">{title}</p>
						<p>{truncatedDesc}</p>
					</div>
					<motion.div
						className="ml-auto mr-4 my-auto"
						initial={false}
						animate={{
							rotate: isOpen ? 180 : 0,
						}}
					>
						<FaChevronCircleDown className=" h-6 w-6" />
					</motion.div>
				</div>
			</motion.div>
			<AnimatePresence>{isOpen && children}</AnimatePresence>
		</>
	);
};

export default AccordionItem;
