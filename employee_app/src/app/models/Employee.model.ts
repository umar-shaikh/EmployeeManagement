export class EmployeeModel {

    employeeId: number;
    name: string;
    contactNo: string;
    email: string;
    city: string;
    state: string;
    pincode: string;
    altContactNo: string;
    address: string;
    designationId: number;
    createdDate: Date | null;
    modifiedDate: Date | null;
    role: string;

    constructor() {

        this.employeeId = 0;
        this.name = '';
        this.contactNo = '';
        this.email = '';
        this.city = '';
        this.state = '';
        this.pincode = '';
        this.altContactNo = '';
        this.address = '';
        this.designationId = 0;
        this.createdDate = null;
        this.modifiedDate = null;
        this.role = '';

    }

}