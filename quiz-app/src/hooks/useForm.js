import { useState } from "react";

 export default function useForm(getFreshModeObject){
    const [values, setValues] = useState(getFreshModeObject());
    const [errors, setErrors] = useState({});

    const handleInputChange = e =>{
        const { name, value} = e.target
        setValues({
            ...values,
            [name]: value
        })
    }
    return{
        values,
        setValues,
        errors,
        setErrors, 
    handleInputChange
    }
 }