import React from 'react'
import { Segment , Form } from 'semantic-ui-react'

const ActivityForm = () => {
    return (
        <div>
            <Segment>
                <Form>
                    <Form.Input placeholder="Title" />
                    <Form.TextArea rows={2} placeholder="Description" />
                    <Form.Input placeholder="Category" />
                    <Form.Input placeholder="Date" type="date" />
                    <Form.Input placeholder="City" />
                    <Form.Input placeholder="Venue" />
                </Form>
            </Segment>
        </div>
    )
}

export default ActivityForm
