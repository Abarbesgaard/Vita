import React, { useState } from 'react';
import { Calendar } from 'react-big-calendar';
import localizer from '../../Services/localizer';
import EventModal from './components/EventModal';
import ModifyEventModal from './components/ModifyEventModal';
import './components/CalendarStyle.css';

export default function CalenderPage() {
    const [events, setEvents] = useState([]);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [showEventModal, setShowEventModal] = useState(false);
    const [showModifyModal, setShowModifyModal] = useState(false);

    const handleSelectEvent = (event) => {
        setSelectedEvent(event);
        setShowEventModal(true);
    };

    const handleModifyEvent = () => {
        setShowEventModal(false);
        setShowModifyModal(true);
    };

    const handleSaveModifiedEvent = (formData) => {
        setEvents(prevEvents => 
            prevEvents.map(event => 
                event === selectedEvent 
                    ? {
                        ...event,
                        title: formData.title,
                        description: formData.description,
                        start: new Date(formData.start),
                        end: new Date(formData.end)
                    }
                    : event
            )
        );
        setShowModifyModal(false);
    };

    return (
        <div>
            <div style={{ height: '600px', overflow: 'hidden' }}>
                <Calendar
                    localizer={localizer}
                    events={events}
                    startAccessor="start"
                    endAccessor="end"
                    style={{ height: '100%' }}
                    onSelectEvent={handleSelectEvent}
                    // ... other Calendar props
                />
            </div>

            <EventModal 
                show={showEventModal}
                event={selectedEvent}
                onClose={() => setShowEventModal(false)}
                onModify={handleModifyEvent}
                onDelete={() => {/* handle delete */}}
            />

            <ModifyEventModal
                show={showModifyModal}
                event={selectedEvent}
                onSubmit={handleSaveModifiedEvent}
                onClose={() => {
                    setShowModifyModal(false);
                    setShowEventModal(true);
                }}
            />
        </div>
    );
} 