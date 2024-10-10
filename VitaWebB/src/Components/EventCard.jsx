import React, { useState, useEffect } from 'react';

const EventCard = ({ show, onClose, onSave, selectedSlot }) => {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [start, setStart] = useState('');
    const [end, setEnd] = useState('');

    useEffect(() => {
        if (selectedSlot) {
            setStart(formatDate(selectedSlot.start));
            setEnd(formatDate(selectedSlot.end));
        }
    }, [selectedSlot]);

    const formatDate = (date) => {
        const offset = date.getTimezoneOffset();
        const adjustedDate = new Date(date.getTime() - offset * 60 * 1000);
        return adjustedDate.toISOString().slice(0, 16);
    };

    const handleSave = () => {
        if (!title.trim() || !description.trim()) {
            alert('Titel og beskrivelse skal udfyldes');
            return;
        }
        const startDate = new Date(start);
        const endDate = new Date(end);
        onSave({
            title: title.trim(),
            description: description.trim(),
            start: new Date(startDate.getTime() + startDate.getTimezoneOffset() * 60 * 1000),
            end: new Date(endDate.getTime() + endDate.getTimezoneOffset() * 60 * 1000)
        });
        setTitle('');
        setDescription('');
        onClose();
    };

    if (!show) {
        return null;
    }

    return (
        <div className="modal-overlay">
            <div className="modal-popup">
                <h2 className="modal-title">Opret Begivenhed</h2>
                <form className="event-form">
                    <div className="form-group">
                        <label htmlFor="title">Titel:</label>
                        <input
                            id="title"
                            type="text"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="description">Beskrivelse:</label>
                        <textarea
                            id="description"
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="start">Start tid:</label>
                        <input
                            id="start"
                            type="datetime-local"
                            value={start}
                            onChange={(e) => setStart(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="end">Slut tid:</label>
                        <input
                            id="end"
                            type="datetime-local"
                            value={end}
                            onChange={(e) => setEnd(e.target.value)}
                        />
                    </div>
                    <div className="button-group">
                        <button type="button" className="btn btn-save" onClick={handleSave}>Save</button>
                        <button type="button" className="btn btn-cancel" onClick={onClose}>Cancel</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default EventCard;