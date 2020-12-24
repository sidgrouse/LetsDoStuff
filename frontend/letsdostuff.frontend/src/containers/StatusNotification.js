import React from "react";
import {toast} from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css';

toast.configure();

export function notifySuccess(message = ''){
    toast.success('🦄 Success!' + message, {
        position: "bottom-center",
        autoClose: 2000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
    });
}

export function notifyError(message = ''){
    toast.error('⚠️ Error! ' + message, {
        position: "bottom-center",
        autoClose: 2000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
    });
}