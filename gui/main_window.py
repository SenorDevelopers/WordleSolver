from constants import WINDOW_WIDTH, WINDOW_HEIGHT, WINDOW_TITLE, GRID_ROWS, GRID_COLS
import tkinter.messagebox
from state import State
import customtkinter
import tkinter

customtkinter.set_appearance_mode("System")  
customtkinter.set_default_color_theme("blue") 

class MainWindow(customtkinter.CTk):

    def __init__(self):
        self.__state = State()
        super().__init__()
        self.__configure_window()   
        self.__configure_grid_layout()  
        self.__configure_left_frame()
        self.__configure_right_frame()

    def __configure_window(self):
        self.title(WINDOW_TITLE)
        self.geometry(f"{WINDOW_WIDTH}x{WINDOW_HEIGHT}")
        self.resizable(False, False)
        self.protocol("WM_DELETE_WINDOW", self.on_closing) 
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(0, weight=1)

    def __configure_left_frame(self): 
        self.frame_left = customtkinter.CTkFrame(master=self,width=180,corner_radius=0)
        self.frame_left.grid(row=0, column=0, sticky="nswe")

        self.frame_left.grid_rowconfigure(0, minsize=10)   # empty row with minsize as spacing
        self.frame_left.grid_rowconfigure(5, weight=1)  # empty row as spacing
        self.frame_left.grid_rowconfigure(8, minsize=20)    # empty row with minsize as spacing
        self.frame_left.grid_rowconfigure(11, minsize=10)  # empty row with minsize as spacing

        self.label_1 = customtkinter.CTkLabel(master=self.frame_left,text="Worlde GUI",text_font=("Roboto Medium", -16))  # font name and size in px
        self.label_1.grid(row=1, column=0, pady=10, padx=10)

        self.button_1 = customtkinter.CTkButton(master=self.frame_left,text="New Game",command=self.new_game)
        self.button_1.grid(row=2, column=0, pady=10, padx=20)

        self.button_2 = customtkinter.CTkButton(master=self.frame_left,text="Next Guess",command=self.next_guess)
        self.button_2.grid(row=3, column=0, pady=10, padx=20)

        self.label_mode = customtkinter.CTkLabel(master=self.frame_left, text="Appearance Mode:")
        self.label_mode.grid(row=9, column=0, pady=0, padx=20, sticky="w")

        self.optionmenu_1 = customtkinter.CTkOptionMenu(master=self.frame_left,values=["Light", "Dark", "System"],command=self.change_appearance_mode)
        self.optionmenu_1.grid(row=10, column=0, pady=10, padx=20, sticky="w")

    def __configure_right_frame(self): 
        self.frame_right = customtkinter.CTkFrame(master=self)
        self.frame_right.grid(row=0, column=1, sticky="nswe", padx=20, pady=20)

        self.frame_right.rowconfigure((0, 1, 2, 3), weight=1)
        self.frame_right.rowconfigure(7, weight=10)
        self.frame_right.columnconfigure((0, 1), weight=1)
        self.frame_right.columnconfigure(2, weight=0)

        self.frame_info = customtkinter.CTkFrame(master=self.frame_right)
        self.frame_info.grid(row=0, column=0, columnspan=2, rowspan=4, pady=20, padx=20, sticky="nsew")

        self.frame_info.rowconfigure(0, weight=1)
        self.frame_info.columnconfigure(0, weight=1)

        self.update()

        self.entry = customtkinter.CTkEntry(master=self.frame_right,width=120,placeholder_text="Enter your guess here")
        self.entry.grid(row=8, column=0, columnspan=2, pady=20, padx=20, sticky="we")

        self.optionmenu_1.set("Dark") 

    def __configure_grid_layout(self):
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(0, weight=1)
        self.frame_left = customtkinter.CTkFrame(master=self, width=180,corner_radius=0)
        self.frame_left.grid(row=0, column=0, sticky="nswe")
        self.frame_right = customtkinter.CTkFrame(master=self)
        self.frame_right.grid(row=0, column=1, sticky="nswe", padx=20, pady=20)

    def new_game(self):
        self.__state.reset_state()
        self.__state.get_opener()
        self.update()

    def next_guess(self):
        pass

    def change_appearance_mode(self, new_appearance_mode):
        customtkinter.set_appearance_mode(new_appearance_mode)

    def on_closing(self):
        self.destroy()

    def update(self):
        for row in range(GRID_COLS):
            for column in range(GRID_COLS):
                self.label_info_1 = customtkinter.CTkLabel(master=self.frame_info,
                                                           text=" " ,
                                                           height=50,
                                                           width=50,
                                                           corner_radius=6,
                                                           fg_color=("white", "gray38"),
                                                           justify=tkinter.LEFT)
                self.label_info_1.grid(column=column, row=row, padx=15, pady=15)
                self.label_info_1.configure(text=f"{self.__state.get_state(row, column)}")

                if self.__state.get_state(row, column) == "":
                    self.label_info_1.configure(fg_color=("grey", "gray38"))
                elif self.__state.get_pattern(row, column) == "1":
                    self.label_info_1.configure(fg_color=("yellow", "gray38"))
                elif self.__state.get_pattern(row, column) == "2":
                    self.label_info_1.configure(fg_color=("green", "gray38"))
                else:
                    self.label_info_1.configure(fg_color=("white", "gray38"))
