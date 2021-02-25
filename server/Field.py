class Field:
    def __init__(self, str):
        str = str.replace("\n", "")
        spl = str.split(';')
        self.type = spl[0]
        self.name = spl[1]
        if self.type == "cont":
            self.max = float(spl[2].split(":")[1])
            self.min = float(spl[3].split(":")[1])
        elif self.type == "disc":
            self.top = spl[2].split(":")[1]
            self.map = eval(spl[3])
            self.categories = list(self.map.values())
        elif self.type == "bol":
            self.max = float(spl[2].split(":")[1])
            self.min = float(spl[3].split(":")[1])

