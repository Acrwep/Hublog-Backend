import React, { useState } from "react";
import { MdRefresh, MdDownload } from "react-icons/md";

import Dropdown from "../../components/dropdown/Dropdown";
import DateRangePicker from "../../components/dateRangePicker/DatePicker";

import { TbCirclesFilled } from "react-icons/tb";
import Summary from "./Summary";
import AttendanceDetail from "./AddendanceDetail";
import DateWiseAttendance from "./DateWiseAttendance";
import withLogin from "../../components/withLogin";

const Attendance = () => {
  // Sample data for charts
  const [activePage, setActivePage] = useState(1);

  const handlePageChange = (pageNumber) => {
    setActivePage(pageNumber);
  };

  return (
    <div className="p-8 max-sm:p-0">
      <div className="flex justify-between items-center">
        <div className="flex justify-start items-center">
          <TbCirclesFilled className="text-2xl text-blue-600" />
          <h2 className="text-xl font-bold ml-2">Attendance</h2>
        </div>
        <ul className="flex w-96 shadow-lg rounded">
          <li
            onClick={() => handlePageChange(1)}
            className={`m-2 p-1 cursor-pointer ${
              activePage === 1 ? "bg-blue-500 text-white" : "bg-gray-200"
            } rounded`}
          >
            Summary
          </li>
          <li
            onClick={() => handlePageChange(2)}
            className={`m-2 p-1 cursor-pointer ${
              activePage === 2 ? "bg-blue-500 text-white" : "bg-gray-200"
            } rounded`}
          >
            Detailed
          </li>
          <li
            onClick={() => handlePageChange(3)}
            className={`m-2 p-1 cursor-pointer ${
              activePage === 3 ? "bg-blue-500 text-white" : "bg-gray-200"
            } rounded`}
          >
            Datewise Attendance
          </li>
        </ul>
      </div>
      <div className="flex justify-between items-center w-full mb-2 max-sm:flex-col max-sm:w-full">
        <div>
          <Dropdown />
        </div>
        <div className="flex justify-end items-center h-20 w-full max-sm:flex-col">
          <div>
            <DateRangePicker />
          </div>
          <div>
            <button className="text-blue-500 p-1 m-1 border border-black rounded-lg">
              <MdDownload />
            </button>
            <button className="text-blue-500 p-1 border border-black rounded-md">
              <MdRefresh />
            </button>
          </div>
        </div>
      </div>
      <div>
        {activePage === 1 && (
          <div>
            <Summary />
            {/* Add your content for page 1 here */}
          </div>
        )}
        {activePage === 2 && (
          <div>
            <AttendanceDetail />
          </div>
        )}
        {activePage === 3 && (
          <div>
            <DateWiseAttendance />
          </div>
        )}
      </div>
    </div>
  );
};

export default withLogin(Attendance);
