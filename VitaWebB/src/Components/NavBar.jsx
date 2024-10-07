import React from 'react';
import { NavLink } from 'react-router-dom';

const NavBar = () => {
    return (
        <nav className="navbar">
            <img className='nav-image' src='https://www.vitahus.dk/wp-content/uploads/Vitahus-Logo-Web.png' alt='Vitahus Logo'/>
            <div className="nav-buttons">
                <NavLink 
                    className={({ isActive }) => isActive ? "nav-button-active" : "nav-button"} 
                    to='/'>Home</NavLink>
                <NavLink 
                    className={({ isActive }) => isActive ? "nav-button-active" : "nav-button"} 
                    to='/videos'>Videos</NavLink>
                <NavLink 
                    className={({ isActive }) => isActive ? "nav-button-active" : "nav-button"} 
                    to='/calendar'>Calendar</NavLink>
            </div> 
        </nav>
    );
};

export default NavBar;