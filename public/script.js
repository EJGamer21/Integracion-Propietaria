// Get table columns and set table rows
function table() {
    return {
        columns: [
            { title: 'Codigo del centro', field: 'codigoDelCentro' },
            { title: 'Nombre del centro', field: 'nombreDelCentro' },
            { title: 'Sector', field: 'sector' },
            { title: 'Provincia', field: 'provincia' },
            { title: 'Matricula', field: 'matricula' },
            { title: 'Nombre del estudiante', field: 'nombreDelEstudiante' },
            { title: 'Asignatura', field: 'asignatura' },
            { title: 'Tanda', field: 'tanda' },
            { title: 'Calificacion', field: 'calificacion' },
            { title: 'Condicion academica', field: 'condicionAcademica' },
            { title: 'Seccion', field: 'seccion' },
            { title: 'Grado', field: 'grado' }
        ],
        rows: [],
        async loadTable() {
            const request = await fetch("http://localhost:5000/api");
            this.rows = await request.json();
        } 
    }
}

// Data and function to manage form
function form() {
    return {
        csvFile: {
            name: "Subir archivo CSV",
            raw: null
        },
        xmlFile: {
            name: "Subir archivo XML",
            raw: null
        },
        setCsvFile(e) {
            const file = e.target.files[0]
            if (!file) return
            this.csvFile = {
                name: file.name,
                raw: file
            }
        },
        setXmlFile(e) {
            const file = e.target.files[0]
            if (!file) return
            this.xmlFile = {
                name: file.name,
                raw: file
            }
        },
        async upload() {
            if (!this.csvFile.raw) {
                return alert("Debes seleccionar un archivo")
            }

            const formData = new FormData();
            formData.append("file", this.csvFile.raw, this.csvFile.name)

            await fetch("http://localhost:5000/api/upload", {
                method: "POST",
                body: formData
            });
        },
        async download() {
            const request = await fetch("http://localhost:5000/api/download");
            const { header, data } = await request.json();

            let csvContent = "data:text/csv;charset=utf-8,";
            csvContent += this.getHeader(header);
            csvContent += data;
            
            const uri = encodeURI(csvContent);

            const link = document.createElement("a");
            link.setAttribute("href", uri);
            link.setAttribute("download", "MINERDd.csv");
            document.body.appendChild(link);

            link.click();
        },
        isLastProperty(property, array) {
            return property === array[array.length - 1];
        },
        getHeader(source = "") {
            let text = "";
            const values = source.split(',');

            const toSentence = (txt) => {
                return txt.replace(/([A-Z])/g, " $1");
            }

            for (const value of values) {
                text += this.isLastProperty(value, values)
                    ? toSentence(value).toUpperCase() + "\n"
                    : toSentence(value).toUpperCase() + ",";
            }

            return text
        },
        async import_xml() {
            if (!this.xmlFile.raw) {
                return alert("Debes seleccionar un archivo")
            }

            const formData = new FormData();
            formData.append("file", this.xmlFile.raw, this.xmlFile.name)

            const { result } = await fetch("http://localhost:5000/api/import", {
                method: "POST",
                body: formData
            });
        },
        async export_xml() {
            const request = await fetch("http://localhost:5000/api/export");
            const file = await request.blob();
            
            const url = window.URL.createObjectURL(file);
            const link = document.createElement("a");

            link.setAttribute("href", url);
            link.setAttribute("download", "export.xml");
            document.body.appendChild(link);

            link.click();
        }
    }
}
